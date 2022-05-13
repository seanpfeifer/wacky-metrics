mergeInto(LibraryManager.library, {
  LoadFileFromBrowser: function (fileCallback) {
    const pickerOpts = {
      types: [
        {
          description: 'CSV',
          accept: {
            'text/*': ['.csv', '.txt']
          }
        },
      ],
      excludeAcceptAllOption: true,
      multiple: false
    };
    window.showOpenFilePicker(pickerOpts).then(fs => {
      fs[0].getFile().then(f => {
        f.text().then(txtData => {
          var bufSize = lengthBytesUTF8(txtData) + 1; // +1 for null byte at end
          var buffer = _malloc(bufSize);
          stringToUTF8(txtData, buffer, bufSize);

          dynCall_vi(fileCallback, [buffer]);
        });
      }
      ).catch(e => console.log(e));
    }).catch(e => console.log(e));
  },
});
