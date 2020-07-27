<template>
  <b-row v-if="category">
    <b-col>
      <div>
        <div class="mb-2">
          <div class="d-flex justify-content-between" v-if="!titleEditMode">
            <h4 class="m-0">{{category.title}}</h4><hr />
            <b-dropdown variant="link" toggle-class="text-decoration-none" no-caret
                        v-if="$auth.loggedIn">
              <template v-slot:button-content>
                <b-icon icon="three-dots-vertical" />
              </template>
              <b-dropdown-item @click="titleEditToggle()"><b-icon icon="pencil" /> {{$t('category.edit-title')}}</b-dropdown-item>
              <b-dropdown-item @click="remove()"><b-icon icon="trash" variant="danger" /> {{$t('common.delete')}}</b-dropdown-item>
            </b-dropdown>
          </div>
          <b-input-group v-else>
            <b-form-input v-model="titleEditValue"></b-form-input>
            <b-input-group-append>
              <b-button variant="success" @click="updateTitle()">{{$t('common.save-changes')}}</b-button>
              <b-button variant="danger" @click="titleEditToggle()">{{$t('common.cancel')}}</b-button>
            </b-input-group-append>
          </b-input-group>
        </div>
        <div>
          <b-icon icon="clock" /> <time>{{category.createdAt | formatDateTime}}</time> |
          <b-icon icon="person" /> <b-link :to="`/user-profile/${category.createdByUser.id}`">{{category.createdByUser.name}}</b-link>
        </div>
        <hr />
        <div class="d-flex justify-content-between" v-if="$auth.loggedIn">
          <span>
            <b-button variant="primary" :to="`/thread/new?cid=${categoryId}`">
              <b-icon icon="plus"></b-icon> {{$t('thread.new')}}
            </b-button>
            <b-button variant="primary" @click="uiNewCategory">
              <b-icon icon="plus"></b-icon> {{$t('category.new-sub')}}
            </b-button>
          </span>
          <span>
            <b-button variant="warning" @click="back">
              <b-icon icon="arrow-left-circle"></b-icon> {{$t('common.return')}}
            </b-button>
          </span>
        </div>

        <div v-if="category.subCategories.length > 0">
          <div class="py-2">{{$t('category.sub')}}</div>
          <b-list-group>
            <CategoryItem v-for="i in category.subCategories" :key="i.id" :data="i" />
          </b-list-group>
        </div>

        <div class="pt-4 pb-2">{{$t('thread.plural')}}</div>
        <b-list-group>
          <ThreadItem v-for="i in category.threads" :key="i.id" :data="i" />
        </b-list-group>
      </div>
    </b-col>
    <b-col cols="4">
      <div>
        <StructureMap
          :parents="category.parentCategories"
          :siblings="category.siblingCategories"
          parentsRouteBase="/category/"
          siblingsRouteBase="/category/"
          siblingsIcon="folder"
          currentId="categoryId" />
      </div>
    </b-col>
  </b-row>
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        category: 'category/details',
      }),
      categoryId() {
        return this.$route.params.category;
      },
    },
    data: () => ({
      titleEditMode: false,
      titleEditValue: '',
    }),
    mounted() {
      this.loadSingleCategory(this.categoryId)
    },
    methods: {
      ...mapActions({
        loadSingleCategory: 'category/loadSingleCategory',
        newCategory: 'category/newCategory',
        deleteCategory: 'category/deleteCategory',
        updateCategoryTitle: 'category/updateCategoryTitle',
      }),
      uiNewCategory() {
        var title = window.prompt(this.$t('category.enter-title'));
        if (title && title.length > 0) {
          var data = { parentCategoryId: this.categoryId, title };
          this.newCategory(data).then(r => {
            this.$router.push(`/category/${r}`)
          }).catch(e => {
            e.showNotification(this)
          })
        }
      },
      titleEditToggle() {
        this.titleEditValue = this.category.title
        this.titleEditMode = !this.titleEditMode
      },
      updateTitle() {
        var data = { id: this.categoryId, title: this.titleEditValue }
        this.updateCategoryTitle(data).then(r => {
          this.titleEditToggle()
        }).catch(e => {
          if (e.serverMessage == 'NOTHING_CHANGED') {
            this.titleEditToggle()
          } else {
            e.showNotification(this)
          }
        })
      },
      remove() {
        this.$bvModal.msgBoxConfirm(this.$t('common.delete-confirm-message'), {
          title: this.$t('common.warning'),
          size: 'sm',
          buttonSize: 'sm',
          okVariant: 'danger',
          okTitle: this.$t('common.im-sure'),
          cancelTitle: this.$t('common.cancel'),
          footerClass: 'p-2',
          hideHeaderClose: false,
          centered: true
        }).then(result => {
          if (result === true) {
            this.deleteCategory(this.categoryId).then(r => {
              this.back()
            }).catch(e => {
              e.showNotification(this)
            })
          }
        })
      },
      back() {
        if (this.category.parentCategoryId)
          this.$router.push(`/category/${this.category.parentCategoryId}`)
        else
          this.$router.push('/')
      }
    }
  }
</script>
