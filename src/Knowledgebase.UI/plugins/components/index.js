import Vue from 'vue'
import VueShowdown from './vue-showdown'

const ComponentsPlugin = function (_Vue, options) {
  _Vue.mixin({
    components: {
      markdown: VueShowdown
    }
  });
}

Vue.use(ComponentsPlugin)
