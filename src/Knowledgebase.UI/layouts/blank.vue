<template>
  <div :dir="direction">
    <nuxt />
  </div>
</template>

<script>
  export default {
    computed: {
      direction() {
        return this.$t('dir')
      }
    }
  }
</script>
