export const state = {
  items: []
}

export const getters = {
  items: state => state.items
}

export const actions = {
  loadTags({ commit }) {
    this.$api.get('/threads/tags').then(r => {
      commit('set_items', r.data)
    })
  }
}

export const mutations = {
  set_items(state, data) {
    state.items = data
  }
}
