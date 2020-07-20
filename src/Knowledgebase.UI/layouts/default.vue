<template>
  <div v-if="loaded">
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
