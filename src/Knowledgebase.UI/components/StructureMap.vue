<template>
  <b-card>
    <b-card-title title-tag="h5">
      <b-icon icon="filter-right" /> {{$t('thread.map')}}
    </b-card-title>
    <b-card-text>
      <div>
        <b-icon icon="house-door" />
        <b-link to="/">{{$t('common.home')}}</b-link>
      </div>
      <div v-for="p,pi in parents" :key="p.id">
        <b-icon icon="blank" v-for="i,ii in (new Array(pi+1))" :key="ii" />
        <b-icon :icon="`arrow-return-${($t('dir') == 'rtl' ? 'left' : 'right')}`" />
        <b-link :to="`${parentsRouteBase}${p.id}`">{{p.name}}</b-link>
      </div>
      <div v-for="s,si in siblings" :key="s.id">
        <b-icon icon="blank" v-for="i,ii in (new Array(parents.length+1))" :key="ii" />
        <b-icon :icon="siblingsIcon" />
        <b-link :to="`${siblingsRouteBase}${s.id}`" :disabled="s.id == currentId">{{s.name}}</b-link>
      </div>
    </b-card-text>
  </b-card>
</template>
<script>
  export default {
    name: 'StructureMap',
    props: {
      parents: { type: Array, default: () => ([]) },
      siblings: { type: Array, default: () => ([]) },
      parentsRouteBase: { type: String, default: '' },
      siblingsRouteBase: { type: String, default: '' },
      siblingsIcon: { type: String, default: '' },
      currentId: { type: String, default: '' },
    }
  }
</script>
