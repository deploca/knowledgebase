
export const state = {
  items: [
    { id: 1, title: 'سیستم عامل لینوکس' },
    { id: 2, title: 'آشنایی، نصب و کار با داکر' },
  ]
}

export const getters = {
  items: state => state.items,
}
