import { toQueryString } from '~/plugins/utils'

export const state = {
  items: [],
  details: null
}

export const getters = {
  items: state => state.items,
  details: state => state.details,
}

export const actions = {
  loadThreads({ commit }, data) {
    return new Promise((resolve, reject) => {
      var params = toQueryString(data)
      this.$api.get('/threads?' + params).then(r => {
        commit('set_items', r.data)
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
  loadSingleThread({ commit }, id) {
    return new Promise((resolve, reject) => {
      this.$api.get('/threads/' + id).then(r => {
        commit('set_details', r.data)
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
  newThread({ dispatch }, data) {
    return new Promise((resolve, reject) => {
      this.$api.post('/threads', data).then(r => {
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
}

export const mutations = {
  set_items(state, data) {
    state.items = data
  },
  set_details(state, data) {
    state.details = data
  }
}
