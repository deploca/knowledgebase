
export const state = {
  items: []
}

export const getters = {
  items: state => state.items,
}

export const actions = {
  loadCategories({ commit }) {
    return this.$api.get('/categories').then(r => {
      commit('setItems', r.data)
    })
  },
  newCategory({ dispatch }, data) {
    return new Promise((resolve, reject) => {
      this.$api.post('/categories', data).then(r => {
        dispatch('loadCategories')
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
}

export const mutations = {
  setItems(state, data) {
    state.items = data;
  }
}
