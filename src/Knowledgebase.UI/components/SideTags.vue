<template>
  <div class="py-2">
    <div class="py-2"><b-icon icon="hash" /> برچسب ها</div>
    <b-list-group>
      <b-list-group-item v-for="i in tags" :key="i.id"
                         class="d-flex justify-content-between align-items-center"
                         :to="`/tag/${i.id}`">
        {{i.name}}
        <b-badge variant="primary" pill>{{i.threadsCount || 0}}</b-badge>
      </b-list-group-item>
      <b-list-group-item class="text-center" v-if="tags.length == 0">
        هیچ برچسبی وجود ندارد
      </b-list-group-item>
    </b-list-group>
  </div>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    name: 'SideTags',
    computed: {
      ...mapGetters({
        tags: 'tag/items',
      }),
    },
  }
</script>
