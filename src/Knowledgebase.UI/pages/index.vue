<template>
  <b-row>
    <b-col>
      <div>
        <div class="d-flex justify-content-between align-items-center">
          <h5><b-icon icon="folder" /> {{$t('category.root')}}</h5>
          <b-button variant="primary" class="mb-2" @click="uiNewCategory">
            <b-icon icon="plus"></b-icon> {{$t('category.new-sub')}}
          </b-button>
        </div>
        <b-list-group>
          <CategoryItem v-for="i in categories" :key="i.id" :data="i" />
        </b-list-group>
      </div>
    </b-col>
    <b-col cols="4">
      <AppSide />
    </b-col>
  </b-row>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        categories: 'category/items',
      }),
    },
    mounted() {
      this.loadCategories({ parent_id: 'root' })
    },
    methods: {
      ...mapActions({
        loadCategories: 'category/loadCategories',
        newCategory: 'category/newCategory',
      }),
      uiNewCategory() {
        var title = window.prompt(this.$t('category.enter-title'));
        if (title && title.length > 0) {
          var data = { parentCategoryId: null, title };
          this.newCategory(data).then(r => {
            this.$router.push(`/category/${r}`)
          }).catch(e => {
            e.showNotification(this)
          })
        }
      }
    }
  }
</script>
