import { toQueryString } from '~/plugins/utils'

export const state = () => ({
  items: [],
  details: null
})

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
        commit('set_latest_version_contents', r.data.contents)
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
  loadSingleThreadContents({ commit, getters }, contentId) {
    return new Promise((resolve, reject) => {
      if (!contentId || contentId == 'latest') {
        commit('set_contents', getters.details.latestVersionContents)
        return resolve()
      }

      this.$api.get('/threads/contents/' + contentId).then(r => {
        commit('set_contents', r.data)
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
  updateThreadTitle({ commit }, data) {
    return new Promise((resolve, reject) => {
      this.$api.put('/threads/title', data).then(r => {
        commit('set_title', data.title)
        return resolve(r.data)
      }).catch(e => reject(e))
    })
  },
  updateThreadContents({ commit }, data) {
    return new Promise((resolve, reject) => {
      this.$api.put('/threads/contents', data).then(r => {
        commit('push_content_version', r.data)
        commit('set_contents', r.data)
        commit('set_latest_version_contents', r.data)
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
  },
  set_title(state, value) {
    state.details.title = value
  },
  set_contents(state, data) {
    state.details.contents = data
  },
  set_latest_version_contents(state, data) {
    state.details.latestVersionContents = data
  },
  push_content_version(state, data) {
    state.details.versions.unshift(data)
  }
}
