<template>
  <b-container fluid="lg" class="py-4">
    <b-card>
      <div class="text-center">
        <h3>{{$t('common.app-name')}}<br /><small>{{$t('views.admin.setup')}}</small></h3>
        <hr />
      </div>
      <div class="text-center" v-if="$auth.loggedIn == false">
        <p>
          This application uses Auth0 for User Accounts Management.
          Before installation, you should login or create an account to countinue.
        </p>
        <b-button @click="login()">Login or Create an account</b-button>
      </div>
      <b-form @submit="onSubmit" v-else>
        <b-form-group :label="$t('views.admin.language')"
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-select v-model="model.locale" :options="uiLocales" @change="onLocaleChange"></b-form-select>
        </b-form-group>

        <b-form-group :label="$t('views.admin.company-name')"
                      :description="$t('views.admin.company-name-desc')"
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-input v-model="model.companyName" required />
        </b-form-group>

        <b-form-group :label="$t('views.admin.admin-email')"
                      :description="$t('views.admin.admin-email-desc')"
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-input :value="`${currentUser.name} (${currentUser.email})`"
                        disabled />
        </b-form-group>

        <!--<b-form-group :label="$t('views.admin.admin-email')"
                :description="$t('views.admin.admin-email-desc')"
                label-cols-sm="4"
                label-cols-lg="3">
    <b-form-input v-model="model.adminEmail"
                  type="email"
                  required />
  </b-form-group>

  <b-form-group :label="$t('views.admin.admin-password')"
                :description="$t('views.admin.admin-password-desc')"
                label-cols-sm="4"
                label-cols-lg="3">
    <b-form-input v-model="model.adminPassword"
                  type="password"
                  required />
  </b-form-group>-->

        <div class="mt-4">
          <b-button type="submit" variant="primary">{{$t('views.admin.setup-submit')}}</b-button>
        </div>
      </b-form>
    </b-card>
  </b-container>
</template>

<script>
  import { mapGetters } from 'vuex'
  export default {
    name: 'setup-view',
    layout: 'blank',
    computed: {
      ...mapGetters({
        locales: 'locales'
      }),
      currentUser() {
        return this.$store.state.auth.user ? this.$store.state.auth.user : {}
      },
      uiLocales() {
        return this.locales.map(x => ({ value: x.code, text: x.name }))
      }
    },
    data: () => ({
      model: {
        companyName: '',
        adminEmail: '',
        adminPassword: '',
        locale: '',
      }
    }),
    mounted() {
      this.model.locale = this.$store.state.locale
    },
    methods: {
      onSubmit(e) {
        e.preventDefault()
        this.$api.post('/admin/setup', this.model).then(r => {
          this.$router.push('/')
        }).catch(e => {
          console.log(e)
        })
      },
      onLocaleChange(value) {
        this.$store.commit('SET_LANG', value)
        this.$i18n.locale = value
      },
      login() {
        this.$auth.login()
      }
    }
  }
</script>
