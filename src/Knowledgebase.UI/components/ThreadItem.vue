<template>
  <b-list-group-item
    class="flex-column align-items-start"
    :to="`/thread/${data.id}`">
    <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">{{data.title}}</h5>
      <small>{{data.updatedAt}}</small>
    </div>

    <p class="mb-1">
      Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.
    </p>

    <div>
      <b-badge to="/tag/os" variant="primary">os</b-badge>
      <b-badge to="/tag/docker" variant="success">docker</b-badge>
    </div>
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