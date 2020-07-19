<template>
  <div class="py-2">
    <div class="py-2">برچسب ها</div>
    <b-list-group>
      <b-list-group-item class="d-flex justify-content-between align-items-center"
        :to="`/tag/operating-system`">
        Cras justo odio
        <b-badge variant="primary" pill>14</b-badge>
      </b-list-group-item>
  
      <b-list-group-item class="d-flex justify-content-between align-items-center">
        Dapibus ac facilisis in
        <b-badge variant="primary" pill>2</b-badge>
      </b-list-group-item>
  
      <b-list-group-item class="d-flex justify-content-between align-items-center">
        Morbi leo risus
        <b-badge variant="primary" pill>1</b-badge>
      </b-list-group-item>
    </b-list-group>
  </div>
</template>

<script>
export default {
  name: 'SideTags'
}
</script>