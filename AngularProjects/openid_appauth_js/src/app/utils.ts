// https://github.com/openid/AppAuth-JS/issues/195#issuecomment-953363001

import { BasicQueryStringUtils } from "@openid/appauth";
import { LocationLike, StringMap } from "@openid/appauth"
/**
 * @class NoHashQueryStringUtils
 *
 * `NoHashQueryStringUtils` extends AppAuth.js' default query string parser
 * (designed for Angular) to never assume `#`s are used for internal routing.
 *
 * This works around a bug where React URLs feature no hash, and so the parser
 * never detects the query string and OAuth parameters.
 */
export class NoHashQueryStringUtils extends BasicQueryStringUtils {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  override parse(input: LocationLike, useHash?: boolean) {
    return super.parse(input, false);
  }
} 