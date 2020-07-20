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
      <b-form-group label="برچسب ها:">
        <multiselect v-model="model.tags"
                     tag-placeholder="این عبارت را بعنوان یک برچسب جدید ثبت کن"
                     placeholder="جستجو یا افزودن یک برچسب جدید"
                     label="name"
                     track-by="id"
                     :options="tagsLocal"
                     :multiple="true"
                     :taggable="true"
                     @tag="addTag">
        </multiselect>
      </b-form-group>
      <b-button type="submit" variant="primary">ثبت مطلب جدید</b-button>
      <b-button variant="warning" @click="cancel">بازگشت</b-button>
    </b-form>
  </div>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        tags: 'tag/items',
      }),
      //uiTags() {
      //  return this.tags.map(x => ({  }))
      //},
    },
    data: () => ({
      model: {
        categoryId: null,
        contents: '',
        tags: []
      },
      tagsLocal: [],
    }),
    mounted() {
      this.tagsLocal = [...this.tags]
    },
    methods: {
      ...mapActions({
        loadTags: 'tag/loadTags',
        loadCategories: 'category/loadCategories',
        newThread: 'thread/newThread',
      }),
      onSubmit(e) {
        e.preventDefault()
        this.model.categoryId = this.$route.query.cid
        this.newThread(this.model).then(r => {
          if (this.model.tags.filter(x => x.id == null).length > 0) {
            this.loadTags();
          }
          this.loadCategories();
          this.$router.push(`/thread/${r}`)
        })
      },
      cancel() {
        this.$router.push(`/category/${this.$route.query.cid}`)
      },
      addTag(newTag) {
        const tag = { name: newTag, id: null }
        this.tagsLocal.push(tag)
        this.model.tags.push(tag)
      }
    }
  }
</script>
