/*Dist copy files*/

module.exports = function(grunt, data){

  return {
    dist:{
      files:[
        { expand: true, src: ['*','!light'], cwd: 'src/html', dest: 'dist/html' },
        { expand: true, src: ['**'], cwd: 'src/html/light', dest: 'dist/light' },
        { expand: true, src: ['**','!css/*'], cwd: 'src/assets', dest: 'dist/html/assets' },
        { expand: true, src: ['**','!main.js'], cwd: 'src/js', dest: 'dist/html/assets/js' },
        { expand: true, src: ['app.js'], cwd: 'src/js', dest: 'dist/light/assets/js' }
      ]
    },
    light:{
    	files:[
			  {expand: true, src: [
          "css/**",
          "img/**",
          "lib/jquery/**",
          "lib/tether/**",
          "lib/bootstrap/**",
          "lib/perfect-scrollbar/**",
          "lib/open-sans/**",
          "lib/raleway/**",
          "lib/stroke-7/**",
          "lib/font-awesome/**",
					], cwd: 'dist/html/assets', dest: 'dist/light/assets' 
				}
    	]
    }
  };
};