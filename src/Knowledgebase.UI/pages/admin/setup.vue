<template>
  <b-container fluid="lg" class="py-4">
    <b-card>
      <div class="text-center">
        <h3>پایگاه دانش<br /><small>نصب برنامه</small></h3>
        <hr />
      </div>
      <b-form @submit="onSubmit">
        <b-form-group label="نام شرکت:"
                      description="نام شرکت، موسسه یا شخصی که نرم افزار متعلق به اوست."
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-input v-model="model.companyName" required />
        </b-form-group>

        <b-form-group label="آدرس ایمیل مدیر سیستم:"
                      description="آدرس ایمیل مدیر سیستم. تمام رویدادهای سیستم نیز به این ایمیل ارسال میشود."
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-input v-model="model.adminEmail"
                        type="email"
                        required />
        </b-form-group>

        <b-form-group label="کلمه عبور مدیر سیستم:"
                      description="کلمه عبور مدیر سیستم. برای مدیریت سیستم این ایمیل و کلمه عبور را فراموش نکنید."
                      label-cols-sm="4"
                      label-cols-lg="3">
          <b-form-input v-model="model.adminPassword"
                        type="password"
                        required />
        </b-form-group>

        <div class="mt-4">
          <b-button type="submit" variant="primary">ثبت اطلاعات و ورود به سیستم</b-button>
        </div>
      </b-form>
    </b-card>
  </b-container>
</template>

<script>
  export default {
    name: 'setup-view',
    layout: 'blank',
    data: () => ({
      model: {
        companyName: '',
        adminEmail: '',
        adminPassword: '',
      }
    }),
    methods: {
      onSubmit(e) {
        e.preventDefault()
        this.$api.post('/admin/setup', this.model).then(r => {
          this.$router.push('/')
        }).catch(e => {
          console.log(e)
        })
      },
    }
  }
</script>
