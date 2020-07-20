<template>
  <b-list-group-item
    class="flex-column align-items-start"
    :to="`/thread/${data.id}`">
    <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">{{data.title}}</h5>
      <small>{{data.updatedAt}}</small>
    </div>

    <!--<p class="mb-1">
      Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.
    </p>-->

    <small>
      <b-icon icon="person" /> مدیر سیستم |
      <b-icon icon="clock" /> <time>{{data.createdAt}}</time> |
      <b-icon icon="hash" />
      <b-badge v-for="i in data.tags" :key="i.id" :to="`/tag/${i.id}`" variant="primary" class="mr-1">{{i.name}}</b-badge>
    </small>
  </b-list-group-item>
</template>
<script>
export default {
  name: 'ThreadItem',
  props: {
    data: { type: Object, default: null }
  }
}
</script>
