export const state = () => ({
  locales: [
    { code: 'en', name: 'English' },
    { code: 'fa', name: 'فارسی' },
  ],
  locale: 'en'
})

export const getters = {
  locales: state => state.locales,
  currentLocale: state => state.locale,
}

export const mutations = {
  SET_LANG(state, locale) {
    if (state.locales.includes(locale)) {
      state.locale = locale
    }
  }
}
