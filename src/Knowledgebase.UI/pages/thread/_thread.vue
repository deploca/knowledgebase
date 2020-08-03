<template>
  <b-row v-if="threadDetails">
    <b-col>
      <div>
        <div class="mb-2">
          <div class="d-flex justify-content-between" v-if="!titleEditMode">
            <h4 class="m-0">{{threadDetails.title}}</h4>
            <b-dropdown variant="link" toggle-class="text-decoration-none" no-caret
                        v-if="$auth.loggedIn">
              <template v-slot:button-content>
                <b-icon icon="three-dots-vertical" />
              </template>
              <b-dropdown-item @click="titleEditToggle()"><b-icon icon="pencil" /> {{$t('thread.edit-title')}}</b-dropdown-item>
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
          <b-icon icon="clock" /> <time>{{threadDetails.createdAt | formatDateTime}}</time> |
          <b-icon icon="person" /> <b-link :to="`/user-profile/${threadDetails.createdByUser.id}`">{{threadDetails.createdByUser.name}}</b-link>
        </div>
        <hr />
        <div class="d-flex justify-content-between mb-2">
          <div v-if="!contentsEditMode">
            <b-form inline>
              <b-button variant="primary" class="mr-2"
                        @click="contentsEditToggle()"
                        :disabled="selectedThreadContentVersionId != threadDetails.latestVersionContents.id"
                        v-if="$auth.loggedIn">
                <b-icon icon="pencil"></b-icon> {{$t('common.edit')}}
              </b-button>
              <b-input-group :prepend="$t('thread.version')">
                <b-form-select v-model="selectedThreadContentVersionId"
                               :options="uiThreadVersions"
                               @change="onThreadVersionChange">
                </b-form-select>
              </b-input-group>
            </b-form>
          </div>
          <div v-else>
            <b-button variant="success" @click="updateContents()">
              <b-icon icon="check2"></b-icon> {{$t('common.save-changes')}}
            </b-button>
            <b-button variant="danger" @click="contentsEditToggle()">
              <b-icon icon="x"></b-icon> {{$t('common.cancel')}}
            </b-button>
          </div>
          <b-button variant="warning" @click="back">
            {{$t('common.return')}} <b-icon icon="arrow-left-circle"></b-icon>
          </b-button>
        </div>
        <div>
          <b-card v-if="!contentsEditMode">
            <markdown :value="threadDetails.contents.contents" />
          </b-card>
          <Editor v-model="contentsEditValue" v-else />
        </div>
      </div>
    </b-col>
    <b-col cols="4">
      <div>
        <b-card class="mb-2">
          <b-card-title title-tag="h5">
            <b-icon icon="hash" /> {{$t('tag.plural')}}
          </b-card-title>
          <b-card-text>
            <b-badge v-for="i in threadDetails.tags" :key="i.id"
                     variant="primary" class="mr-1"
                     :to="`/tag/${i.id}`">
              {{i.name}}
            </b-badge>
          </b-card-text>
        </b-card>
        <StructureMap
          :parents="threadDetails.parentCategories"
          :siblings="threadDetails.siblingThreads"
          parentsRouteBase="/category/"
          siblingsRouteBase="/thread/"
          siblingsIcon="files"
          currentId="threadId" />
      </div>
    </b-col>
  </b-row>
  <!--<br /><br />
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
  </b-media>-->
</template>

<script>
  import { mapGetters, mapActions } from 'vuex'
  export default {
    computed: {
      ...mapGetters({
        threadDetails: 'thread/details',
      }),
      threadId() {
        return this.$route.params.thread;
      },
      uiThreadVersions() {
        return this.threadDetails.versions.map(x => ({
          value: x.id,
          text: `${x.createdByUser.name} @ ${this.$dateTime.formatDateTime(x.createdAt)}`
        }))
      }
    },
    data: () => ({
      titleEditMode: false,
      titleEditValue: '',
      contentsEditMode: false,
      contentsEditValue: '',
      selectedThreadContentVersionId: null,
    }),
    mounted() {
      this.loadSingleThread(this.threadId).then(() => {
        this.selectedThreadContentVersionId = this.threadDetails.contents.id
      })
    },
    methods: {
      ...mapActions({
        loadSingleThread: 'thread/loadSingleThread',
        loadSingleThreadContents: 'thread/loadSingleThreadContents',
        updateThreadTitle: 'thread/updateThreadTitle',
        updateThreadContents: 'thread/updateThreadContents',
        deleteThread: 'thread/deleteThread',
      }),
      titleEditToggle() {
        this.titleEditValue = this.threadDetails.title
        this.titleEditMode = !this.titleEditMode
      },
      updateTitle() {
        var data = { id: this.threadId, title: this.titleEditValue }
        this.updateThreadTitle(data).then(r => {
          this.titleEditToggle()
        }).catch(e => {
          if (e.serverMessage == 'NOTHING_CHANGED') {
            this.titleEditToggle()
          } else {
            e.showNotification(this)
          }
        })
      },
      contentsEditToggle() {
        this.contentsEditValue = this.threadDetails.contents.contents
        this.contentsEditMode = !this.contentsEditMode
      },
      updateContents() {
        var data = { id: this.threadId, contents: this.contentsEditValue }
        this.updateThreadContents(data).then(contents => {
          this.selectedThreadContentVersionId = contents.id
          this.contentsEditToggle()
        }).catch(e => {
          if (e.serverMessage == 'NOTHING_CHANGED') {
            this.contentsEditToggle()
          } else {
            e.showNotification(this)
          }
        })
      },
      onThreadVersionChange(value) {
        this.loadSingleThreadContents(value || 'latest')
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
            this.deleteThread(this.threadId).then(r => {
              this.back()
            })
          }
        })
      },
      back() {
        this.$router.push(`/category/${this.threadDetails.category.id}`)
      }
    }
  }
</script>
