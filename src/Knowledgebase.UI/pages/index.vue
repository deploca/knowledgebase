<template>
  <div>
    <div class="py-2">دسته های اصلی</div>
    <b-list-group>
      <CategoryItem v-for="i in categories" :key="i.id" :data="i" />
    </b-list-group>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  computed: {
    ...mapGetters({
      categories: 'category/items'
    })
  }
}
</script>
