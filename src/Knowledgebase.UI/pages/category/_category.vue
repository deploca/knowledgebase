<template>
  <div v-if="category">
    <h4 class="m-0">{{category.title}}</h4><hr />
    <div class="d-flex justify-content-between">
      <span>
        <b-button variant="primary" :to="`/thread/new?cid=${categoryId}`">
          <b-icon icon="plus"></b-icon> مطلب جدید
        </b-button>
        <b-button variant="primary" @click="uiNewCategory">
          <b-icon icon="plus"></b-icon> زیر دسته جدید
        </b-button>
      </span>
      <span>
        <b-button variant="warning" @click="gotoParentCategory">
          <b-icon icon="arrow-left-circle"></b-icon> بازگشت
        </b-button>
      </span>
    </div>
    <div v-if="subCategories.length > 0">
      <div class="py-2">زیر دسته ها</div>
      <b-list-group>
        <CategoryItem v-for="i in subCategories" :key="i.id" :data="i" />
      </b-list-group>
    </div>

    <div class="pt-4 pb-2">مطالب</div>
    <b-list-group>
      <ThreadItem v-for="i in threads" :key="i.id" :data="i" />
    </b-list-group>
  </div>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        categories: 'category/items',
        threads: 'thread/items',
      }),
      categoryId() {
        return this.$route.params.category;
      },
      category() {
        return this.categories.filter(x => x.id == this.categoryId)[0]
      },
      subCategories() {
        return this.categories.filter(x => x.parentCategoryId == this.categoryId)
      }
    },
    mounted() {
      this.loadThreads({ category_id: this.categoryId })
    },
    methods: {
      ...mapActions({
        loadThreads: 'thread/loadThreads',
        newCategory: 'category/newCategory',
      }),
      uiNewCategory() {
        var title = window.prompt('عنوان دسته را تایپ نمایید:');
        if (title && title.length > 0) {
          var data = { parentCategoryId: this.categoryId, title };
          this.newCategory(data).then(r => {
            this.$router.push(`/category/${r}`)
          })
        }
      },
      gotoParentCategory() {
        if (this.category.parentCategoryId)
          this.$router.push(`/category/${this.category.parentCategoryId}`)
        else
          this.$router.push('/')
      }
    }
  }
</script>
