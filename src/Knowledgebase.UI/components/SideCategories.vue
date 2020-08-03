<template>
  <div class="py-2">
    <div class="py-2"><b-icon icon="folder" /> {{$t('category.plural')}}</div>
    <b-list-group>
      <b-list-group-item v-for="i in categories" :key="i.id"
                         class="d-flex justify-content-between align-items-center"
                         :to="`/category/${i.id}`">
        {{i.title}}
        <!--<b-badge variant="primary" pill>{{i.threadsCount || 0}}</b-badge>-->
      </b-list-group-item>
      <b-list-group-item class="text-center" v-if="categories.length == 0">
        {{$t('category.nothing-found')}}
      </b-list-group-item>
    </b-list-group>
  </div>
</template>

<script>
  import { mapGetters } from 'vuex'
  import CategoryItem from './CategoryItem'
  export default {
    name: 'SideCategories',
    components: { CategoryItem },
    computed: {
      ...mapGetters({
        categories: 'category/items'
      })
    }
  }
</script>
