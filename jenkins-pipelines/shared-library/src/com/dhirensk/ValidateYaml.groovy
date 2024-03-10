package com.dhirensk

/**
* Jenkins Shared Library Class to validate Yaml Files in the BitBucket Pull Request
*/
class ValidateYaml implements Serializable {

  def steps
  def env
  def diffstat
  def bitbucketAccessTokenCredentialId

  ValidateYaml(steps, env, bitbucketAccessTokenCredentialId) {
    this.steps = steps
    this.env = env
    this.bitbucketAccessTokenCredentialId = bitbucketAccessTokenCredentialId
  }

  def validate(payload) {
    def status = true
    def jsonPayload = steps.readJSON(text: payload)
    def diffStatUrl = jsonPayload['pullrequest']['links']['diffstat']['href']
    def diffstat = steps.withEnv(["diffStatUrl=$diffStatUrl"]) {
      def diffStatOut = steps.withCredentials([
        steps.string(credentialsId: bitbucketAccessTokenCredentialId, variable: 'TOKEN')
        ]) {
        def diffStatResponse =  steps.sh(returnStdout: true, script: '''
                    set -x
                    curl -L --request GET --url "$diffStatUrl" --header "Authorization: Bearer $TOKEN"
                    ''')
        return diffStatResponse
      }
      return diffStatOut
    }
    def diffStatJsonString = steps.withEnv(["diffstat=$diffstat"]) {
      return steps.sh(returnStdout: true, script: '''#!/bin/bash
                jq -r '.values[].new.path' <<< "$diffstat"
                ''').split('\n')
    }
    for (file in diffStatJsonString) {
      if (file.toLowerCase().endsWith('.yaml') || file.toLowerCase().endsWith('.yml')) {
        try {
          steps.readYaml file: file
          steps.echo("Valid Yaml file: $file")
        }
        catch (Exception e) {
          steps.echo("Error reading: $file")
          steps.echo(e.toString())
          steps.echo(e.stackTrace.toString())
          status = false
        }
      }
    }
    return status
  }

}
