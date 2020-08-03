<template>
  <b-list-group-item class="flex-column align-items-start"
                     :to="`/category/${data.id}`">
    <div class="d-flex w-100 justify-content-between">
      <h5 class="mb-1">{{data.title}}</h5>
      <!--<small>3 days ago</small>-->
    </div>

    <!--<p class="mb-1">
      Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.
    </p>-->

    <small>
      <b-icon icon="folder" /> {{data.subCategoriesCount}} |
      <b-icon icon="files" /> {{data.threadsCount}} |
      <b-icon icon="person" /> {{data.createdByUser.name}}
    </small>
  </b-list-group-item>
</template>
<script>
  export default {
    name: 'CategoryItem',
    props: {
      data: { type: Object, default: null },
    }
  }
</script>
