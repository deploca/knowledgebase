import { toQueryString } from '~/plugins/utils'

export const state = () => ({
  items: [],
  details: null,
})

export const getters = {
  items: state => state.items,
  details: state => state.details,
}

export const actions = {
  loadCategories({ commit }, data) {
    var params = toQueryString(data)
    return this.$api.get('/categories?' + params).then(r => {
      commit('set_items', r.data)
    })
  },
  loadSingleCategory({ commit }, parentCategoryId) {
    return new Promise((resolve, reject) => {
      commit('set_details', null)
      this.$api.get('/categories/' + (parentCategoryId || '')).then(r => {
        commit('set_details', r.data)
        return resolve(r.data)
      }).catch(e => reject(e))
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
  set_items(state, data) {
    state.items = data;
  },
  set_details(state, data) {
    state.details = data;
  }
}
