/* Based on
 * https://auth.nuxtjs.org/guide/scheme.html#creating-your-own-scheme
 * https://github.com/nuxt-community/auth-module/blob/master/lib/schemes/oauth2.js
 */

import LocalScheme from '@nuxtjs/auth/lib/schemes/local'
import { normalizePath, encodeValue } from '@nuxtjs/auth/lib/core/utilities'
import { clientAppBaseUrl, apiServerBaseUrl } from '@/plugins/api'

export default class AppAuthScheme extends LocalScheme {

  constructor(auth, options) {
    super(auth, options)
    this.$api = this.$auth.ctx.$api;
    this.$router = this.$auth.ctx.app.router;
  }

  async mounted() {
    // Handle callbacks on page load
    const signinCallbackHandled = await this._handleSigninCallback()
    if (!signinCallbackHandled)
      await this.$auth.fetchUserOnce()
  }

  login(endpoint) {
    // return path of client app
    this.$auth.$storage.setUniversal(this.name + '.returnUrl', this.$auth.ctx.route.path)

    const returnUrl = encodeValue(`${clientAppBaseUrl}/signedin`)
    window.location.href = `${apiServerBaseUrl}/auth/login?returnUrl=${returnUrl}`
  }

  logout(endpoint) {
    window.location.href = `${apiServerBaseUrl}/auth/logout`
  }

  // Override `fetchUser` method of `local` scheme
  async fetchUser(endpoint) {
    await this.reset();
    const fetchResult = await this.$api.get('/common/user-info')
    this.$auth.setUser(fetchResult.data)
  }

  async _handleSigninCallback() {
    // Handle callback only for specified route
    if (normalizePath(this.$auth.ctx.route.path) !== normalizePath('/signedin')) {
      return
    }
    // Callback flow is not supported in server side
    if (process.server) {
      return
    }

    await this.$auth.fetchUserOnce()

    const returnUrl = this.$auth.$storage.getUniversal(this.name + '.returnUrl')
    setTimeout(() => {
      this.$router.push(returnUrl)
    })
    return true
  }
}
