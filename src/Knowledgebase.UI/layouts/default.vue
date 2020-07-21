<template>
  <div :dir="direction" v-if="loaded">
    <AppHeader />
    <b-container fluid="lg">
      <b-row>
        <b-col>
          <nuxt />
        </b-col>
        <b-col cols="4">
          <AppSide />
        </b-col>
      </b-row>
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
          // start application
          this.loadCategories();
          this.loadTags();

          // set configurations
          this.$store.commit('SET_LANG', appSettings['Locale'])
          this.$i18n.locale = appSettings['Locale']

          this.loaded = true;
        }
      });
    },
    methods: {
      ...mapActions({
        loadAppSettings: 'appSettings/loadAppSettings',
        loadCategories: 'category/loadCategories',
        loadTags: 'tag/loadTags',
      })
    }
  }</script>
