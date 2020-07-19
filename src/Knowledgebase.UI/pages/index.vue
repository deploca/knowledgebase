<template>
  <div>
    <div class="py-2">دسته های اصلی</div>
    <b-list-group>
      <CategoryItem v-for="i in rootCategories" :key="i.id" :data="i" />
    </b-list-group>
    <b-button variant="primary" class="mb-2" @click="uiNewCategory">
      <b-icon icon="plus"></b-icon> زیر دسته جدید
    </b-button>
  </div>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  import { BIcon, BIconPlus } from 'bootstrap-vue'
  export default {
    components: { BIcon, BIconPlus },
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
        var title = window.prompt('عنوان دسته را تایپ نمایید:');
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
