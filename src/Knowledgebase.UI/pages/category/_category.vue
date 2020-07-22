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
        <b-card>
          <b-card-title title-tag="h5">
            <b-icon icon="filter-right" /> {{$t('thread.map')}}
          </b-card-title>
          <b-card-text>
            <div v-for="c,ci in category.parentCategories" :key="c.id">
              <span v-if="ci > 0">
                <b-icon icon="blank" v-for="i,ii in (new Array(ci-1))" :key="ii" />
                <b-icon icon="arrow90deg-up" />
              </span>
              <b-link :to="`/category/${c.id}`">{{c.name}}</b-link>
            </div>
            <div v-for="c,ci in category.siblingCategories" :key="c.id">
              <b-icon icon="blank" v-for="i,ii in (new Array((category.parentCategories.length > 0 ? category.parentCategories.length : 1) - 1))" :key="ii" />
              <b-icon icon="folder" />
              <b-link :to="`/category/${c.id}`">{{c.name}}</b-link>
            </div>
          </b-card-text>
        </b-card>
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
