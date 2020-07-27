
export const state = () => ({
  items: []
})

export const getters = {
  items: state => state.items,
}

export const actions = {
  loadUsers({ commit }) {
    return new Promise((resolve, reject) => {
      this.$api.get('/users').then(r => {
        commit('set_items', r.data)
        return resolve(r.data)
      })
    })
  }
}

export const mutations = {
  set_items(state, data) {
    state.items = data
  }
}
