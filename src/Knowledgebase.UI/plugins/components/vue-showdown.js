import Vue from 'vue'
import VueShowdown from 'vue-showdown'

export default Vue.extend({
  props: {
    value: { type: String, default: '' },
  },
  render(createElement) {
    return createElement(VueShowdown.VueShowdown, {
      'class': 'markdown',
      props: {
        markdown: this.value,
        flavor: 'github',
        options: {
          emoji: true,
          openLinksInNewWindow: true,
        },
      }
    })
  }
})
