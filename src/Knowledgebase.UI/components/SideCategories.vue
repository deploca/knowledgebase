<template>
  <div class="py-2">
    <div class="py-2">دسته ها</div>
    <b-list-group>
      <CategoryItem v-for="i in categories" :key="i.id" :data="i" />
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
