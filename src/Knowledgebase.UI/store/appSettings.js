export const state = {
  items: []
}

export const getters = {
  items: state => state.items,
}

export const actions = {
  loadAppSettings({ commit }) {
    return new Promise((resolve, reject) => {
      this.$api.get('/common/app-settings').then(r => {
        commit('set_items', r.data)
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  }
}

export const mutations = {
  set_items(state, data) {
    state.items = data
  }
}
