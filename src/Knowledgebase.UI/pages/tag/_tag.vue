<template>
  <div>
    <h4 class="m-0">{{tag.name}}</h4><hr />

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
        tags: 'tag/items',
        threads: 'thread/items',
      }),
      tagId() {
        return this.$route.params.tag;
      },
      tag() {
        return this.tags.filter(x => x.id == this.tagId)[0]
      },
    },
    mounted() {
      this.loadThreads({ tag_id: this.tagId })
    },
    methods: {
      ...mapActions({
        loadThreads: 'thread/loadThreads',
      }),
    }
  }
</script>
