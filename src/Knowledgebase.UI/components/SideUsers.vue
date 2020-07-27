<template>
  <div class="py-2">
    <div class="py-2"><b-icon icon="person" /> {{$t('user.actives')}}</div>
    <b-list-group>
      <b-list-group-item class="d-flex align-items-center"
                         v-for="i in users" :key="i.id">
        <UserAvatar :user="i" class="mr-3" />
        <span>{{i.name}}</span>
      </b-list-group-item>
    </b-list-group>
  </div>
</template>

<script>
  import { mapGetters } from 'vuex'
  export default {
    name: 'SideUsers',
    computed: {
      ...mapGetters({
        users: 'user/items'
      })
    },
  }
</script>
