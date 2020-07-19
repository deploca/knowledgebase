<template>
  <div>
    <div>
      <b-button variant="primary" class="mb-2" to="/thread/new">
        <b-icon icon="plus"></b-icon> مطلب جدید
      </b-button>
      <b-button variant="primary" class="mb-2" to="/category/new">
        <b-icon icon="plus"></b-icon> زیر دسته جدید
      </b-button>
    </div>
    <div class="py-2">زیر دسته ها</div>
    <b-list-group>
      <CategoryItem v-for="i in categories" :key="i.id" :data="i" />
    </b-list-group>

    <div class="pt-4 pb-2">مطالب</div>
    <b-list-group>
      <ThreadItem v-for="i in threads" :key="i.id" :data="i" />
    </b-list-group>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { BIcon, BIconPlus } from 'bootstrap-vue'
export default {
  components: { BIcon, BIconPlus },
  computed: {
    ...mapGetters({
      categories: 'category/items',
      threads: 'thread/items'
    })
  }
}
</script>