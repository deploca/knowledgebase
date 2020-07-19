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

<script>import { mapActions } from 'vuex'
  export default {
    mounted() {
      this.loadCategories();
    },
    methods: {
      ...mapActions({
        loadCategories: 'category/loadCategories',
      })
    }
  }</script>
