<template>
  <div :dir="direction" v-if="loaded">
    <AppHeader />
    <b-container fluid="lg">
      <nuxt />
    </b-container>
    <AppFooter />
  </div>
</template>

<script>
  import { mapActions } from 'vuex'
  export default {
    computed: {
      direction() {
        return this.$t('dir')
      }
    },
    data: () => ({
      loaded: false,
    }),
    mounted() {
      this.loadAppSettings().then(appSettings => {
        if (!appSettings || Object.keys(appSettings).length == 0) {
          // first run, goto setup page
          this.$router.push('/admin/setup')
        } else {
          // set configurations
          this.$store.commit('SET_LANG', appSettings['Locale'])
          this.$i18n.locale = appSettings['Locale']

          // load initial data
          this.loadTags();
          this.loaded = true;
        }
      });
    },
    methods: {
      ...mapActions({
        loadAppSettings: 'appSettings/loadAppSettings',
        loadTags: 'tag/loadTags',
      })
    }
  }
</script>
