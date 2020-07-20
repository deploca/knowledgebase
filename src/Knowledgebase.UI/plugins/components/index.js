import Vue from 'vue'
import { BootstrapVueIcons } from 'bootstrap-vue'
import VueShowdown from './vue-showdown'
import Multiselect from 'vue-multiselect'
import 'vue-multiselect/dist/vue-multiselect.min.css'

const ComponentsPlugin = function (_Vue, options) {
  _Vue.mixin({
    components: {
      markdown: VueShowdown,
      multiselect: Multiselect
    }
  });
}

Vue.use(ComponentsPlugin)
Vue.use(BootstrapVueIcons)
