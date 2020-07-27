<template>
  <b-avatar :text="text" :src="src"></b-avatar>
</template>
<script>
  export default {
    name: 'UserAvatar',
    props: {
      user: { type: Object, default: () => null }
    },
    computed: {
      src() {
        return this.user.picture && this.user.picture.length > 0 ? this.user.picture : null
      },
      text() {
        if (!this.src) {
          return String(this.user.name.indexOf(' ') > 0 ?
            this.user.name[0] + this.user.name.substr(this.user.name.indexOf(' ') + 1, 1) :
            this.user.name.substr(0, 2)).toUpperCase()
        }
        return null
      }
    }
  }
</script>
