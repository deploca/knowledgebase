<template>
  <div>
    <h4>مطلب جدید</h4><hr />
    <b-form @submit="onSubmit">
      <b-form-group label="موضوع:">
        <b-form-input v-model="model.title"
                      placeholder="Enter the thread title"
                      required />
      </b-form-group>
      <b-form-group label="متن:">
        <b-form-textarea v-model="model.contents"
                         placeholder="Enter something..."
                         rows="3" max-rows="6">
        </b-form-textarea>
      </b-form-group>
      <b-button type="submit" variant="primary">ثبت مطلب جدید</b-button>
      <b-button variant="warning" @click="cancel">بازگشت</b-button>
    </b-form>
  </div>
</template>

<script>
  import { mapActions } from 'vuex'
  export default {
    data: () => ({
      model: {
        categoryId: null,
        contents: '',
      }
    }),
    methods: {
      ...mapActions({
        newThread: 'thread/newThread',
      }),
      onSubmit(e) {
        e.preventDefault()
        this.model.categoryId = this.$route.query.cid
        this.newThread(this.model).then(r => {
          this.$router.push(`/thread/${r}`)
        })
      },
      cancel() {
        this.$router.push(`/category/${this.$route.query.cid}`)
      }
    }
  }
</script>
