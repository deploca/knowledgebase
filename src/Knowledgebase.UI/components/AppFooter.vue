<template>
  <footer class="footer py-4">
    <b-container fluid="lg">
      <div class="row align-items-center justify-content-md-between">
        <div class="col-md-6">
          <div class="copyright">
            &copy; {{year}}, {{$t('common.copyright')}}
          </div>
        </div>
        <div class="col-md-6">
          <!--<ul class="nav nav-footer justify-content-end">
              <li class="nav-item">
                  <nuxt-link to="/" class="nav-link" rel="noopener"><i class="fa fa-home"></i> خانه</nuxt-link>
              </li>
              <li class="nav-item">
                  <a href="https://console.deploca.com/" class="nav-link" rel="noopener"><i class="fa fa-code"></i> ورود</a>
              </li>
              <li class="nav-item">
                  <a class="nav-link" href="https://github.com/deploca" target="_blank"><i class="fab fa-github"></i> گیت هاب</a>
              </li>
              <li class="nav-item">
                  <a class="nav-link" href="https://www.linkedin.com/company/deploca" target="_blank"><i class="fab fa-linkedin"></i> لینکداین</a>
              </li>
          </ul>-->
        </div>
      </div>
    </b-container>
  </footer>
</template>
<script>
  export default {
    name: 'AppFooter',
    data() {
      return {
        year: new Date().getFullYear()
      }
    }
  };
</script>
