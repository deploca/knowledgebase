<template>
  <div>
    <div class="d-flex justify-content-between">
      <h3>{{details.title}}</h3>
      <b-button variant="primary" class="mb-2"
        @click="$router.push(`/category/${details.category.id}`)">
        بازگشت <b-icon icon="arrow-left-circle"></b-icon>
      </b-button>
    </div>
    <hr />
    <markdown :value="details.contents"/>
    <br /><br />
    <h5>بحث و گفتگو</h5><hr />
    <b-media>
      <template v-slot:aside>
        <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
      </template>

      <h5 class="mt-0">Media Title</h5>
      <p>
        Cras sit amet nibh libero, in gravida nulla. Nulla vel metus scelerisque ante sollicitudin.
        Cras purus odio, vestibulum in vulputate at, tempus viverra turpis. Fusce condimentum nunc
        ac nisi vulputate fringilla. Donec lacinia congue felis in faucibus.
      </p>
      <p>
        Donec sed odio dui. Nullam quis risus eget urna mollis ornare vel eu leo. Cum sociis natoque
        penatibus et magnis dis parturient montes, nascetur ridiculus mus.
      </p>

      <b-media>
        <template v-slot:aside>
          <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
        </template>

        <h5 class="mt-0">Nested Media</h5>
        <p class="mb-0">
          Fusce condimentum nunc ac nisi vulputate fringilla. Donec lacinia congue felis in
          faucibus.
        </p>
      </b-media>
    </b-media>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { BIcon, BIconArrowLeftCircle } from 'bootstrap-vue'
export default {
  components: { BIcon, BIconArrowLeftCircle },
  computed: {
      ...mapGetters({
        details: 'thread/details',
      })
  }
}
</script>