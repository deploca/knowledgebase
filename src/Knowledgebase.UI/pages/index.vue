<template>
  <div>
    <div class="py-2">{{$t('category.root')}}</div>
    <b-list-group>
      <CategoryItem v-for="i in rootCategories" :key="i.id" :data="i" />
    </b-list-group>
    <br />
    <b-button variant="primary" class="mb-2" @click="uiNewCategory">
      <b-icon icon="plus"></b-icon> {{$t('category.new-sub')}}
    </b-button>
  </div>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        categories: 'category/items'
      }),
      rootCategories() {
        return this.categories.filter(x => x.parentCategoryId == null)
      },
    },
    mounted() {
    },
    methods: {
      ...mapActions({
        newCategory: 'category/newCategory',
      }),
      uiNewCategory() {
        var title = window.prompt(this.$t('category.enter-title'));
        if (title && title.length > 0) {
          var data = { parentCategoryId: null, title };
          this.newCategory(data).then(r => {
            this.$router.push(`/category/${r}`)
          })
        }
      }
    }
  }
</script>
