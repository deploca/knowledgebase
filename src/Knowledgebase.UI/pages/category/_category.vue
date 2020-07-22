<template>
  <b-row v-if="category">
    <b-col>
      <div>
        <h4 class="m-0">{{category.title}}</h4><hr />
        <div class="d-flex justify-content-between">
          <span>
            <b-button variant="primary" :to="`/thread/new?cid=${categoryId}`">
              <b-icon icon="plus"></b-icon> {{$t('thread.new')}}
            </b-button>
            <b-button variant="primary" @click="uiNewCategory">
              <b-icon icon="plus"></b-icon> {{$t('category.new-sub')}}
            </b-button>
          </span>
          <span>
            <b-button variant="warning" @click="gotoParentCategory">
              <b-icon icon="arrow-left-circle"></b-icon> {{$t('common.return')}}
            </b-button>
          </span>
        </div>
        <div v-if="category.subCategories.length > 0">
          <div class="py-2">{{$t('category.sub')}}</div>
          <b-list-group>
            <CategoryItem v-for="i in category.subCategories" :key="i.id" :data="i" />
          </b-list-group>
        </div>

        <div class="pt-4 pb-2">{{$t('thread.plural')}}</div>
        <b-list-group>
          <ThreadItem v-for="i in category.threads" :key="i.id" :data="i" />
        </b-list-group>
      </div>
    </b-col>
    <b-col cols="4">
      <div>
        <StructureMap
          :parents="category.parentCategories"
          :siblings="category.siblingCategories"
          parentsRouteBase="/category/"
          siblingsRouteBase="/category/"
          siblingsIcon="folder"
          currentId="categoryId" />
      </div>
    </b-col>
  </b-row>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        category: 'category/details',
      }),
      categoryId() {
        return this.$route.params.category;
      },
    },
    mounted() {
      this.loadSingleCategory(this.categoryId)
    },
    methods: {
      ...mapActions({
        loadSingleCategory: 'category/loadSingleCategory',
        newCategory: 'category/newCategory',
      }),
      uiNewCategory() {
        var title = window.prompt(this.$t('category.enter-title'));
        if (title && title.length > 0) {
          var data = { parentCategoryId: this.categoryId, title };
          this.newCategory(data).then(r => {
            this.$router.push(`/category/${r}`)
          }).catch(e => {
            e.showNotification(this)
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
