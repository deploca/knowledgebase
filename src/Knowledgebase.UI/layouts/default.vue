<template>
  <div>
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
    mounted() {
      this.loadAppSettings().then(appSettings => {
        if (!appSettings || Object.keys(appSettings).length == 0) {
          // first run, goto setup page
          this.$router.push('/admin/setup')
        } else {
          // start application
          this.loadCategories();
        }
      });
    },
    methods: {
      ...mapActions({
        loadAppSettings: 'appSettings/loadAppSettings',
        loadCategories: 'category/loadCategories',
      })
    }
  }</script>
