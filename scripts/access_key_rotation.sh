#!/bin/bash
# run aws saml authentication
# update the list of users to ignore
# the script will only check iam users who have only 1 access-key with age over 90.
# To test uncomment line 13 and add a new iam user and generate an access key. Add genuine users to ignore list on line 17.

prd=12345678
tst=23456789
dev=01234567

declare -i cutofftime
cutofftime=$(date -u +%s --date "-90 days")
#for testing immediate accesskey rotation uncomment below
#cutofftime=$(date -u +%s --date "+1 days")

# add any iam users to ignore from accesskey rotation
ignore_users=(my_test_user my_user2)
secret_arn=arn:aws:secretsmanager:ap-south-1:12345678:secret:prd/iam-users
account_id=$(aws sts get-caller-identity --query Account --output text)

#set account_name
case $account_id in
    "$prd") account_name=prd
    ;;
    "$tst") account_name=tst
    ;;
    "$dev") account_name=dev
    ;;
    *)  echo "Account not in the list"
        exit 1
    ;;
esac
printf -- '-%.0s' {1..100} && printf '\n'
echo "Rotate access-keys in $account_name account: $account_id"
printf -- '-%.0s' {1..100} && printf '\n\n'

# get existing secret and copy it into backup 

current_secret_keys=($(aws secretsmanager get-secret-value --secret-id "$secret_arn" | jq -r '.SecretString | fromjson | keys_unsorted[]'))
if [ $? -ne 0 ]
then
    exit 1
fi
current_secret_values=($(aws secretsmanager get-secret-value --secret-id "$secret_arn" | jq -r '.SecretString | fromjson | values[]'))

#echo "${current_secret_keys[@]}"
#echo "${current_secret_values[@]}"
iam_users=($(aws iam list-users | jq -r '.Users[].UserName'))

printf -- '-%.0s' {1..100} && printf '\n'
for user in "${ignore_users[@]}"
do
    for idx in "${!iam_users[@]}"
    do
        iam_user="${iam_users[$idx]}"
        if [[ "$iam_user" == "$user" ]]
        then
            echo "ignoring iam-user: $user"
            unset 'iam_users[$idx]'
        fi
    done
done
printf -- '-%.0s' {1..100} && printf '\n\n'
printf -- '-%.0s' {1..100} && printf '\n'
echo "rotate access-keys for following iam users: " "${iam_users[@]}"
printf -- '-%.0s' {1..100} && printf '\n\n'
# Rotate access-keys active since 90 days

for iam_user in "${iam_users[@]}"
do  
    echo "Checking access-keys for user =>: $iam_user"
    declare -a access_keys
    #stripping UTC offset hrs as fromdateiso8601 only supports UTC
    # e.g. 2020-12-26T08:07:53.017Z supported , 2020-12-26T08:07:53+05:30 not supported
    
    mapfile -t access_keys < <(aws iam list-access-keys --user-name "$iam_user" | jq -r '.AccessKeyMetadata[]| select(.Status=="Active" and (.CreateDate | strptime("%Y-%m-%dT%H:%M:%S%z") | todate | fromdateiso8601 < '"$cutofftime"'))| .AccessKeyId')
    #TODO: uncomment below and comment above
    #access_keys=($(aws iam list-access-keys --user-name $iam_user | jq -r '.AccessKeyMetadata[]| select(.Status=="Active" and (.CreateDate | fromdateiso8601 > "$cutofftime"))| .AccessKeyId'))
    

    # Currently Only handle iam-users with 1 active keys. for example caap_cdp_kubernetes has 2 access keys and should be manually rotated
    if [ "${#access_keys[@]}" -eq 1 ]
    then
        old_access_key="${access_keys[0]}"
        if ! aws iam update-access-key --access-key-id "$old_access_key" --status Inactive --user-name "$iam_user"
        then 
            echo "error in inactivating access-key: $old_access_key"
        else
            echo "1. Access key" "$old_access_key" "inactivated"
            echo "2. Creating new access key"
            create_key=$( (aws iam create-access-key --user-name "$iam_user") 2>&1)
            if [ $? -eq 0 ]
            then
                new_key=($(echo "$create_key" | jq -r '.AccessKey| .["UserName","Status","CreateDate","SecretAccessKey","AccessKeyId"]'))
                new_accesskeyid="${new_key[4]}"
                new_secretaccesskey="${new_key[3]}"
                #echo "${new_key[@]}"
                found=false
                for idx in "${!current_secret_keys[@]}"
                do
                    key_id="${current_secret_keys[$idx]}"
                    if [[ "$key_id" == "$old_access_key" ]]
                    then
                        found=true
                        current_secret_keys[$idx]="$new_accesskeyid"
                        current_secret_values[$idx]="$new_secretaccesskey"
                    fi
                done
                if [[ "$found" == false ]]
                then
                    current_secret_keys+=("$new_accesskeyid")
                    current_secret_values+=("$new_secretaccesskey")
                fi
                #echo "${current_secret_keys[@]}"
                #echo "${current_secret_values[@]}"
                new_secret_value="{"
                for idx in "${!current_secret_keys[@]}"
                do 
                    if [ "$idx" -ne $(("${#current_secret_keys[@]}"-1)) ]
                    then
                        new_secret_value+="\"${current_secret_keys[$idx]}\":\"${current_secret_values[$idx]}\","
                    else
                        new_secret_value+="\"${current_secret_keys[$idx]}\":\"${current_secret_values[$idx]}\"}"
                    fi
                    
                done
                #echo "$new_secret_value"
                # update secrets manager
                echo "3. Updating the new access key in SecretsManager"
                update_secret=$(aws secretsmanager update-secret --secret-id "$secret_arn" --secret-string "$(echo "$new_secret_value" |jq -r @json)" 2>&1)
                if [ $? -eq 0 ]
                then
                    echo "  3.1 New access key updated in the SecretsManager"
                    # delete inactivated key
                    delete_key=$( (aws iam delete-access-key --user-name "$iam_user" --access-key-id "$old_access_key") 2>&1)
                    if [ $? -eq 0 ]
                    then
                        echo "  3.2 Access key rotated successfully for iam user" "$iam_user"
                        echo "      Old access-key-id:""$old_access_key"" new access-key-id:""$new_accesskeyid"
                    else
                        echo "  Error deleting access-key"
                        echo "$delete_key"
                        echo "  3.2 Delete the old access key:" "$old_access_key" "manually for iam-user" "$iam_user"
                    fi
                else
                    echo "  3.1 Failed to update access key in SecretsManager."
                    echo "$update_secret"
                    echo "  3.2 Deleting the new key and Reverting to old key for now"
                    delete_new=$( (aws iam delete-access-key --user-name "$iam_user" --access-key-id "$new_accesskeyid") 2>&1)
                    restore_old=$( (aws iam update-access-key --access-key-id "$old_access_key" --status Active --user-name "$iam_user") 2>&1)

                fi
                
            else
                echo "Failed to create new Access key"
                echo "Error:""$create_key"
                echo "reactivating existing key"
                $(aws iam update-access-key --access-key-id "$old_access_key" --status Active --user-name "$iam_user")   
            fi 
            
        fi
    else
        echo "Number of Active Access keys with Age > 90 days: ${#access_keys[@]} .Skipping if not equal to 1"
    fi
    printf '\n'
done
