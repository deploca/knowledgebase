import moment from 'moment'
import momentJalali from 'moment-jalaali'

export default ({ store }, inject) => {
  const dateTimeHelpers = new DateTimeHelpers(store)
  inject('dateTime', dateTimeHelpers)
}

class DateTimeHelpers {
  constructor(store) {
    this.store = store
  }

  isFarsi() {
    return this.store.state.locale == 'fa'
  }


  formatDate(value) {
    if (this.isFarsi()) {
      return momentJalali(value).format('jYYYY/jMM/jDD');
    }

    return moment(value).format('YYYY/MM/DD');
  }

  formatDateTime(value) {
    if (this.isFarsi()) {
      return momentJalali(value).format('jYYYY/jMM/jDD - HH:mm');
    }

    return moment(value).format('YYYY/MM/DD - HH:mm');
  }


  today() {
    return this.formatDate(new Date())
  }

  now() {
    return this.formatDateTime(new Date())
  }
}
