<template>
  <header class="bg-primary mb-4">
    <b-container fluid="lg">
      <b-navbar toggleable="lg" type="dark">
        <b-navbar-brand to="/">{{companyName}} - {{$t('common.app-name')}}</b-navbar-brand>
        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
        <b-collapse id="nav-collapse" is-nav>
          <!--<b-navbar-nav>
            <b-nav-item to="/marketplace">بازارچه</b-nav-item>
            <b-nav-item to="/developers">توسعه دهندگان</b-nav-item>
            <b-nav-item to="/docs">مستندات</b-nav-item>
          </b-navbar-nav>-->
          <!-- Right aligned nav items -->
          <b-navbar-nav class="ml-auto">
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
    </b-container>
  </header>
</template>

<script>
  import { mapGetters } from 'vuex'
  export default {
    name: 'AppHeader',
    computed: {
      ...mapGetters({
        appSettings: 'appSettings/items'
      }),

      companyName() {
        return this.appSettings['CompanyName']
      }
    }
  }
</script>
