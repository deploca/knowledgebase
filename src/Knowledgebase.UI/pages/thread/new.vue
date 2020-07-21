<template>
  <div>
    <h4>{{$t('thread.new')}}</h4><hr />
    <b-form @submit="onSubmit">
      <b-form-group :label="$t('thread.title')">
        <b-form-input v-model="model.title"
                      :placeholder="$t('thread.enter-title')"
                      required />
      </b-form-group>
      <b-form-group :label="$t('thread.contents')">
        <b-form-textarea v-model="model.contents"
                         :placeholder="$t('thread.enter-contents')"
                         rows="3" max-rows="6">
        </b-form-textarea>
      </b-form-group>
      <b-form-group :label="$t('tag.plural')">
        <multiselect v-model="model.tags"
                     :tag-placeholder="$t('thread.add-this-as-a-new-tag')"
                     :placeholder="$t('thread.search-or-add-a-new-tag')"
                     label="name"
                     track-by="id"
                     :options="tagsLocal"
                     :multiple="true"
                     :taggable="true"
                     @tag="addTag">
        </multiselect>
      </b-form-group>
      <b-button type="submit" variant="primary">{{$t('thread.new')}}</b-button>
      <b-button variant="warning" @click="cancel">{{$t('common.return')}}</b-button>
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
