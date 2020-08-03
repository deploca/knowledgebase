<template>
  <textarea ref="editor"></textarea>
</template>
<script>
  import EasyMDE from 'easymde'
  export default {
    name: 'Editor',
    props: {
      value: { type: String, default: '' }
    },
    data: () => ({
      instance: null,
      container: null,
    }),
    mounted() {
      this.init();
    },
    beforeDestroy() {
      this.container.remove()
    },
    methods: {
      init() {
        var self = this;
        this.instance = new EasyMDE({
          element: this.$refs.editor,
          autoDownloadFontAwesome: false,
          initialValue: this.value,
          status: false
        })
        this.instance.codemirror.on("change", function () {
          self.$emit('input', self.instance.value())
        })
        this.instance.codemirror.setOption('direction', this.$t('dir'))
        window.setTimeout(() => {
          this.container = this.$refs.editor.nextSibling;
        })
      }
    }
  }
</script>
